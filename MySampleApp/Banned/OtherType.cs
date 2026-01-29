using System.Collections.Generic;
using System.Linq;

namespace MySampleApp.Banned {
    public class OtherType {
        public void WriteLog() {
            Console.WriteLine($"Log from {this.GetType().Name}");
        }
    }
}
