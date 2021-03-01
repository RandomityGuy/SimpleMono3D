using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.UI
{
    public interface ILocationDescription
    {
        object GetDescription();
    }

    public interface ILocationDescription<T> : ILocationDescription
    {
        new T GetDescription();
    }

    public enum Alignment
    {
        TopLeft,
        TopMiddle,
        TopRight,
        Left,
        Middle,
        Right,
        BottomLeft,
        BottomMiddle,
        BottomRight
    }

    public class AlignmentDescription : ILocationDescription<Alignment>
    {
        public Alignment alignment;

        public Alignment GetDescription()
        {
            return alignment;
        }

        object ILocationDescription.GetDescription()
        {
            return alignment;
        }

        public AlignmentDescription(Alignment alignment)
        {
            this.alignment = alignment;
        }
    }
}
