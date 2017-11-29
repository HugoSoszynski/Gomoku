using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku {
    public interface IAI {
        Tuple<uint, uint> MakeMove(Board board);
    }
}
