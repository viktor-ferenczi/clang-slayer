using Sandbox.ModAPI;

namespace ClangSlayer
{
    public class Derivative
    {
        private readonly double minPeriod;
        
        private double previous;
        private double timestamp;
        private double derivative;
        private bool valid;

        public bool Valid => valid;
        public double Dt => valid ? derivative : 0.0;

        public Derivative(double v, double period = 0.1)
        {
            minPeriod = period;
            Reset(v);
        }

        public void Reset(double v)
        {
            previous = v;
            timestamp = MyAPIGateway.Session.ElapsedPlayTime.TotalSeconds;
            valid = false;
        }

        public void Update(double v)
        {
            var t = MyAPIGateway.Session.ElapsedPlayTime.TotalSeconds;
            if (t < timestamp + minPeriod)
            {
                return;
            }

            derivative = (v - previous) / (t - timestamp);
            valid = true;

            previous = v;
            timestamp = t;
        }
    }
}