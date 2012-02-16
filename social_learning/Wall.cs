using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace social_learning
{
    public class Wall
    {
        private readonly int _id;
        public float X1 { get; set; }
        public float Y1 { get; set; }
	    public float X2 { get; set; }
	    public float Y2 { get; set; }
        public float slope { get; set; }
        public float b { get; set; }
        public int Id { get { return _id; } }

        public Wall(int id)
        {
            _id = id;
        }

	public void getFormula(){
        if (this.X1 == this.X2)
            this.slope = float.MaxValue;
        else
		    this.slope = (this.Y2-this.Y1)/(this.X2-this.X1);
		
        this.b = this.Y2-slope*this.X2;
	}

    /**
     * Parameter: x,y coordinate of an agent. v is velocity of an agent.
     **/
	public bool checkCollision(IAgent agent){
		getFormula();
        
        float X = agent.X;
        float Y = agent.Y;
        float V = agent.Velocity;
        
        //velocities of x and y
        float vX = V * (float)(Math.Cos(agent.Orientation * Math.PI / 180.0));
        float vY = V * (float)(Math.Sin(agent.Orientation * Math.PI / 180.0));

        //region check
        if ((X > X1 + vX && X > X2 + vX) || (X < X1 - vX && X < X2 - vX)
            || (Y > Y1 + vY && Y > Y2 + vY) || (Y < Y1 - vY && Y < Y2 - vY))
        {
            return false;
            
        }
    
        float collisionNum = (Y - (this.slope * X + this.b));
        float prevCollisionNum = ((Y-vY) - (this.slope * (X-vX) + this.b));

        //checking jumping over the wall 
        if ((collisionNum <= 0 && prevCollisionNum >= 0) || (collisionNum >= 0 && prevCollisionNum <= 0))
        {
            return true;
        }

        return false;
        //return (Y - (this.slope*X + this.b)) == 0;

	}

        public void Reset(){
			this.X1 = 0;
			this.Y1 = 0;
			this.X2 = 0;
			this.Y2 = 0;
		}
    }
}
