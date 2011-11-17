﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpNeat.Phenomes;

namespace social_learning
{
    public class NeuralAgent : Agent
    {
        public IBlackBox Brain { get; set; }
        
        public NeuralAgent(IBlackBox brain)
        {
            Brain = brain;
        }

        protected override float[] getRotationAndVelocity(double[] sensors)
        {
            Brain.ResetState();

            // Convert the sensors into an input array for the network
            for (int i = 0; i < sensors.Length; i++)
                Brain.InputSignalArray[i] = sensors[i];

            // Activate the network
            Brain.Activate();

            var output = Brain.OutputSignalArray[0];

            // [0,1] -> [-180,180]
            var orientation = (float)(output-0.5) * 120;

            var velocityDelta = (float)(output - 0.5) * 2;

            //MessageBox.Show(result.ToString());

            return new float[] { orientation, velocityDelta };
        }

        public override void Reset()
        {
            Brain.ResetState();
        }
    }
}
