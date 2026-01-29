using System.Collections.Generic;
using System.Linq;

namespace MyLib;
public class Module1 {
    public void WriteLog() {
        Console.WriteLine($"Log from {this.GetType().Name}");
    }
}
