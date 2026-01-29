using System.Collections.Generic;
using System.Linq;

namespace MySampleApp.MyLib;
public class Module2 {
    public void WriteLog() {
        Console.WriteLine($"Log from {this.GetType().Name}");
    }
}
