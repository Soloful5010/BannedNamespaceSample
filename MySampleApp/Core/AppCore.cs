using MyLib;
using MySampleApp.Banned;
using MySampleApp.MyLib;
using System.Collections.Generic;
using System.Linq;

namespace MySampleApp.Core;

public class AppCore {
    public void Run() {
        var mod1 = new Module1();
        mod1.WriteLog();

        var mod2 = new Module2();
        mod2.WriteLog();

        var banned = new BannedType();
        banned.WriteLog();

        var bannedOther = new OtherType();
        bannedOther.WriteLog();
    }
}
