using BlockchainTestApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainTestApp.RunTests
{
    public class ExitTest : RunTestBase
    {
        /// <inheritdoc/>
        public override string RunTestName => "Run and Exit Test";

        /// <inheritdoc/>
        public override RunAction RunAction => RunAction.Exit;

        /// <inheritdoc/>
        public override void Run(object[]? args)
        {
            if (args?.Length > 0)
            {
                IRunTest? rt = null;
                var addArgs = new List<object>();

                foreach (var arg in args)
                {
                    if (arg is IRunTest rt1)
                    {
                        if (rt == null)
                            rt = rt1;
                    }
                    else
                    {
                        addArgs.Add(arg);
                    }
                }

                if (rt != null)
                {
                    rt.Run(addArgs.ToArray());
                }
            }
        }
    }
}
