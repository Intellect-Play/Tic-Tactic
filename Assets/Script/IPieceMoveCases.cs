using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script
{
    public interface IPieceMoveCases
    {
        public void AddToList();
        void MoveStart(Action onMoveComplete);
        public void MoveEnd();
        public void RemoveFromList();

    }
}
