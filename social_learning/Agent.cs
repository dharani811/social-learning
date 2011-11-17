﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace social_learning
{
    public abstract class Agent : IAgent
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Orientation { get; set; }
        public float Velocity { get; set; }
        public double Fitness { get; set; }
        public float MaxVelocity { get { return 5f; } }

        protected abstract float[] getRotationAndVelocity(double[] sensors);

        public void Step(double[] sensors)
        {
            var output = getRotationAndVelocity(sensors);

            Orientation += output[0];
            Orientation %= 360;

            Velocity += output[0];
            Velocity = Math.Min(5, Math.Max(0, Velocity));

            X += Velocity * (float)(Math.Cos(Orientation * Math.PI / 180.0));
            Y += Velocity * (float)(Math.Sin(Orientation * Math.PI / 180.0));
        }

        public abstract void Reset();
    }
}
