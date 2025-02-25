using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisModel.Persistence;
using static TetrisModel.Model.Shape;
using static TetrisModel.Model.Ship;

namespace TetrisModel.Model
{
    public class Shape
    {
        public enum Shapes
        {
            Square,
            Line,
            L,
            J,
            Roof,
            S,
            Z
        };
        #region Properties
        public event EventHandler<ShapeStepEventArgs>? MeteorStepEvent;

        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public List<(int, int)> ShapeElements { get; private set; }

        public int Id { get; private set; }
        public int State { get; private set; }

        public int RtoX {  get; private set; }
        public int RtoY { get; private set; }

        public Shapes ActShape { get; set; }

        #endregion

        #region Constructors
        public Shape(List<(int, int)> list, int id, int state)
        {
            ShapeElements = list;
            Id = id;
            State = state;
        }
        #endregion

        #region Public methods
        public void StepShape()
        {
            MeteorStepEvent?.Invoke(this, new ShapeStepEventArgs());
        }

        public void StepTo()
        {
            for (int i = 0; i < ShapeElements.Count; i++)
            {
                // Új tuple létrehozása az Item1 értékének növelésével
                var newElement = (ShapeElements[i].Item1 + 1, ShapeElements[i].Item2);

                // Az új tuple hozzáadása az eredeti lista helyére
                ShapeElements[i] = newElement;
            }
        }

        public void ChangeState()
        {
            State = (State + 1) % 4;
        }
        #endregion
    }
}
