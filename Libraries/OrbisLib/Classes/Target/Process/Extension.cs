using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisSuite
{
    public class Extension
    {
        private Target Target;
        private Memory Memory;

        public Extension(Target Target, Memory Memory)
        {
            this.Target = Target;
            this.Memory = Memory;
        }
    }
}
