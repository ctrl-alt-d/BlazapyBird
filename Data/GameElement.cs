using System;

namespace BlazapyBird.Data
{
    public class GameElement
    {
        protected static readonly Random getrandom = new Random();
        private Guid key = Guid.NewGuid();
        public string Key => key.ToString();
        public string Name {get; set;} = "";
        public double X {set; get; }
        public double Y {set; get; }
        public virtual int Width {set; get; }
        public virtual int Height {set; get; }
        public double _Mass {set; get; }
        public double _EatedMass {set; get; } = 0;
        public virtual bool EatableByMySelf {set; get; } = false;
        public long Mass => (int)_Mass;
        public virtual long MaxMass {set; get; } = 20000;

        public long CssX =>Convert.ToInt32(X-X/2);
        public long CssY =>Convert.ToInt32(Y-Y/2);
        public string CssClass => this.GetType().Name.ToLower();
        public virtual string CssStyle => $@"
            top: {CssY}px;
            left: {CssX}px;
            width: {Width}px;
            height: {Height}px;
            ";
    }

}