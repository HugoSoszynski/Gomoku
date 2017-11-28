using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku {
    interface IAI {
        Tuple<uint, uint> MakeMove(Board board);
    }
}
