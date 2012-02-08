using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace social_learning
{
    public class Wall
    {
        private readonly int _id;
        public float X { get; set; }
        public float Y { get; set; }
		public float X2 { get; set; }
		public float Y2 { get; set; }
        public int Id { get { return _id; } }

        public Wall(int id)
        {
            _id = id;
        }

        public abstract void Reset(){
			this.X = 0;
			this.Y = 0;
			this.X2 = 0;
			this.Y2 = 0;
		}
    }
}
