﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace social_learning
{
    public class World
    {
        private Random random = new Random();
        const int SENSORS_PER_PLANT_TYPE = 8;

        public IEnumerable<IAgent> Agents { get; set; }
        public double AgentHorizon { get; set; }

        /// <summary>
        /// nom nom nom
        /// </summary>
        public IList<Plant> Plants { get; set; }

        public IEnumerable<PlantSpecies> PlantTypes { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }

        public World(IEnumerable<IAgent> agents, IEnumerable<PlantSpecies> species, int height, int width, int numPlantsPerSpecies)
        {
            Agents = agents;
            PlantTypes = species;
            Height = height;
            Width = width;

            // Randomly populate the world with plants
            Plants = new List<Plant>();
            foreach (var s in species)
                for (int i = 0; i < numPlantsPerSpecies; i++)
                    Plants.Add(new Plant(s) { X = random.Next() % width, Y = random.Next() % height });
        }

        public void Step()
        {
            foreach (var agent in Agents)
            {
                var sensors = calculateSensors(agent);
                agent.Step(sensors);
                if (agent.X > Height)
                    agent.X = Width;
                if (agent.Y > Height)
                    agent.Y = Height;
                if (agent.X <= 0)
                    agent.X = 1;
                if (agent.Y <= 0)
                    agent.Y = 1;
                foreach (var plant in Plants)
                    if (calculateDistance(agent, plant) < plant.Species.Radius && plant.AvailableForEating(agent))
                    {
                        // Eat the plant
                        plant.EatenBy(agent);
                        agent.Fitness += plant.Species.Reward;

                        // TODO: Update the population if the reward was good/bad
                    }
            }

        }

        private double[] calculateSensors(IAgent agent)
        {
            double[] sensors = new double[PlantTypes.Count() * SENSORS_PER_PLANT_TYPE];

            // For every plant
            foreach (var plant in Plants)
            {
                // if the plant isn't available for eating then we do not activate the sensors
                if (!plant.AvailableForEating(agent))
                    continue;

                // Calculate the distance to the object from the agent
                var dist = calculateDistance(agent, plant);

                // If it's too far away for the agent to see
                if (dist > AgentHorizon)
                    continue;

                // Identify the appropriate sensor
                var sIdx = getSensorIndex(agent, plant);

                // Add 1/distance to the sensor
                sensors[sIdx] += 1.0 / dist;
            }
            return sensors;
        }

        private int getSensorIndex(IAgent agent, Plant plant)
        {
            double pos = Math.Atan((plant.Y - agent.Y) / (plant.X - agent.X));

            double startSensor = (agent.Orientation - 90) % 360;
            double sensorWidth = 180.0 / (double)SENSORS_PER_PLANT_TYPE;

            for (int i = 0; i < SENSORS_PER_PLANT_TYPE; i++)
                if ((startSensor + i * sensorWidth) % 360 < pos && (startSensor + (i + 1) * sensorWidth) % 360 >= pos)
                    return plant.Species.SpeciesId * SENSORS_PER_PLANT_TYPE + i;

            throw new Exception("Something went wrong! (Eli screwed up the formula)");
        }

        private static double calculateDistance(IAgent agent, Plant plant)
        {
            return Math.Sqrt((agent.X - plant.X) * (agent.X - plant.X) + (agent.Y - plant.Y) * (agent.Y - plant.Y));
        }

   }
}
