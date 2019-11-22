using Microsoft.AspNetCore.Components;
using BlazapyBird.Data;
using BlazapyBird.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace BlazapyBird.Pages
{

    public class IndexBase: ComponentBase
    {
        [Inject] protected Universe Universe {get; set; }        

        protected Queue<KeyboardEventArgs> KeyPressed = new Queue<KeyboardEventArgs>();

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                MainGame( Render );
            }
        }

        protected async Task Render()
        {
            await InvokeAsync(StateHasChanged);
            await Task.Delay(Universe.FPS_DELAY);
        }

        protected async void MainGame(Func<Task> render)
        {
            var score = 0;
            var playerIndex = 0;
            var loopIter = 0;

            var playerIndexGen = new Cycle<int>(new [] {0, 1, 2, 1}).GetEnumerator();

            var playerx = Convert.ToInt32( Universe.SCREENWIDTH * 0.2);
            var playery = Convert.ToInt32((Universe.SCREENHEIGHT - Universe.GetPlayerHeight / 2));

            var basex = 0;
            var baseShift = Universe.GetBaseWidth - Universe.GetBackgroundWidth;

            var newPipe1 = getRandomPipe();
            var newPipe2 = getRandomPipe();

            // list of upper pipes
            var upperPipes = new List<Dictionary<string,int>>()
            {
                new Dictionary<string,int>() 
                {
                    ["x"] = Universe.SCREENWIDTH + 200, 
                    ["y"] = newPipe1[0]["y"] 
                },  
                new Dictionary<string,int>() 
                {
                    ["x"] = Universe.SCREENWIDTH + 200 + (Universe.SCREENWIDTH / 2), 
                    ["y"] = newPipe2[0]["y"] 
                }, 
            };

            // list of lowerpipe
            var lowerPipes = new List<Dictionary<string,int>>()
            {
                new Dictionary<string,int>() 
                {
                    ["x"] = Universe.SCREENWIDTH + 200, 
                    ["y"] = newPipe1[1]["y"] 
                },  
                new Dictionary<string,int>() 
                {
                    ["x"] = Universe.SCREENWIDTH + 200 + (Universe.SCREENWIDTH / 2), 
                    ["y"] = newPipe2[1]["y"] 
                }, 
            };

            var pipeVelX = -4;

            // player velocity, max velocity, downward accleration, accleration on flap
            var playerVelY    =  -9   ;// player's velocity along Y, default same as playerFlapped
            var playerMaxVelY =  10   ;// max vel along Y, max descend speed
            var playerAccY    =   1   ;// players downward accleration
            var playerRot     =  45   ;// player's rotation
            var playerVelRot  =   3   ;// angular speed
            var playerRotThr  =  20   ;// rotation threshold
            var playerFlapAcc =  -9   ;// players speed on flapping
            var playerFlapped = false ;// True when player flaps

            var random = new Random();
            var randPlayer = random.Next(0, Universe.PLAYERS_LIST.Count() - 1);
            var player_images = new [] {
                Universe.PLAYERS_LIST[randPlayer][0],
                Universe.PLAYERS_LIST[randPlayer][1],
                Universe.PLAYERS_LIST[randPlayer][2],
            };

            while (true)
            {
                while (KeyPressed.Any())
                {
                    var k = KeyPressed.Dequeue();
                    if (k.Key == "KeyUp" || k.Key == "Space"  )
                    {
                        if (playery > -2 * Universe.GetPlayerHeight)
                        {
                            playerVelY = playerFlapAcc;
                            playerFlapped = true;
                            //SOUNDS['wing'].play()
                        }
                    }
                }


                var crashTest = CheckCrash( ( x: playerx, y: playery, index: playerIndex ),
                                            upperPipes, lowerPipes);
                                    
                if (crashTest.collPipe) return; // simplified.

                var playerMidPos = playerx + Universe.GetPlayerWidth / 2;

                // check for score
                foreach(var pipe in upperPipes)
                {
                    var pipeMidPos = pipe["x"] + Universe.GetPipeWidth / 2;
                    if (pipeMidPos <= playerMidPos && playerMidPos < pipeMidPos + 4)
                    {
                        score += 1;
                        //SOUNDS['point'].play()                    
                    }
                }

                // playerIndex basex change
                if ((loopIter + 1) % 3 == 0)
                {
                    playerIndexGen.MoveNext();
                    playerIndex = playerIndexGen.Current;
                    loopIter = (loopIter + 1) % 30;
                    basex = -((-basex + 100) % baseShift);
                }

                // rotate the player
                if (playerRot > -90)
                {
                    playerRot -= playerVelRot;
                }

                // player's movement
                if (playerVelY < playerMaxVelY && !playerFlapped)
                {
                    playerVelY += playerAccY;
                }
                    
                if (playerFlapped)
                {
                    playerFlapped = false;
                    // more rotation to cover the threshold (calculated in visible rotation)
                    playerRot = 45;
                }

                var playerHeight = Universe.GetPlayerHeight;
                playery += new int[] { playerVelY, Convert.ToInt32( Universe.BASEY - playery - playerHeight) }.Min();

                // move pipes to left
                foreach( var (uPipe, lPipe) in upperPipes.Zip(lowerPipes) )
                {
                    uPipe["x"] += pipeVelX;
                    lPipe["x"] += pipeVelX;
                }

                // add new pipe when first pipe is about to touch left of screen
                if ( !upperPipes.Any() || (  0 < upperPipes[0]["x"] &&  upperPipes[0]["x"] < 5 ) )
                {
                    var newPipe = getRandomPipe();
                    upperPipes.Add(newPipe[0]);
                    lowerPipes.Add(newPipe[1]);
                }

                // remove first pipe if its out of the screen
                if (upperPipes[0]["x"] < Universe.GetPipeWidth )
                {
                    upperPipes.RemoveAt(0);
                    lowerPipes.RemoveAt(0);
                }   

                // print

                Universe.ToRender.Clear();
                Universe.ToRender.Add(
                    new Printable( 0, 0, Universe.CurrentBackgroundImage )
                );

                foreach( var (uPipe, lPipe) in upperPipes.Zip(lowerPipes))
                {
                    Universe.ToRender.Add(
                        new Printable( uPipe["x"], uPipe["y"], Universe.PIPES_LIST[0] )
                    );
                    Universe.ToRender.Add(
                        new Printable( lPipe["x"], lPipe["y"], Universe.PIPES_LIST[1] )
                    );
                }

                Universe.ToRender.Add(
                    new Printable( basex, Convert.ToInt32( Universe.BASEY), Universe.IMAGES["base"]  )
                );

                // print score so player overlaps the score
                // showScore(score): simplify

                var visibleRot = playerRotThr;
                if (playerRot <= playerRotThr)
                {
                    visibleRot = playerRot;
                }
                
                //  playerSurface = pygame.transform.rotate(IMAGES['player'][playerIndex], visibleRot)  Simplify

                var ocell = new Printable( playerx, playery,  player_images[playerIndex] );
                Universe.ToRender.Add(ocell);

                await render();

            }
            
            /*

        
        SCREEN.blit(playerSurface, (playerx, playery))

        pygame.display.update()
        FPSCLOCK.tick(FPS)            
             */
        }

        private (bool collPipe, bool collBase) CheckCrash((int x, int y, int index) p, List<Dictionary<string, int>> upperPipes, List<Dictionary<string, int>> lowerPipes)
        {
            /*
                """returns True if player collders with base or pipes."""
                pi = player['index']
                player['w'] = IMAGES['player'][0].get_width()
                player['h'] = IMAGES['player'][0].get_height()

                # if player crashes into ground
                if player['y'] + player['h'] >= BASEY - 1:
                    return [True, True]
                else:

                    playerRect = pygame.Rect(player['x'], player['y'],
                                player['w'], player['h'])
                    pipeW = IMAGES['pipe'][0].get_width()
                    pipeH = IMAGES['pipe'][0].get_height()

                    for uPipe, lPipe in zip(upperPipes, lowerPipes):
                        # upper and lower pipe rects
                        uPipeRect = pygame.Rect(uPipe['x'], uPipe['y'], pipeW, pipeH)
                        lPipeRect = pygame.Rect(lPipe['x'], lPipe['y'], pipeW, pipeH)

                        # player and upper/lower pipe hitmasks
                        pHitMask = HITMASKS['player'][pi]
                        uHitmask = HITMASKS['pipe'][0]
                        lHitmask = HITMASKS['pipe'][1]

                        # if bird collided with upipe or lpipe
                        uCollide = pixelCollision(playerRect, uPipeRect, pHitMask, uHitmask)
                        lCollide = pixelCollision(playerRect, lPipeRect, pHitMask, lHitmask)

                        if uCollide or lCollide:
                            return [True, False]

                return [False, False]            
             */
             return (collPipe: false, collBase: false);
        }

        private Dictionary<string,int>[] getRandomPipe()
        {
            //returns a randomly generated pipe
            // y of gap between upper and lower pipe

            Random random = new Random();
            var gapY = random.Next(0, Convert.ToInt32(Universe.BASEY * 0.6 - Universe.PIPEGAPSIZE) );
            gapY += Convert.ToInt32(Universe.BASEY * 0.2);
            var pipeHeight = Universe.GetPipeHeight;
            var pipeX = Universe.SCREENWIDTH + 10;
            var pipe = new [] {
                new Dictionary<string,int>() {["x"] = pipeX, ["y"] = gapY - pipeHeight },  // upper pipe
                new Dictionary<string,int>() {["x"] = pipeX, ["y"] = gapY + Universe.PIPEGAPSIZE }, // lower pipe
            };
            return pipe;
        }
    }

}