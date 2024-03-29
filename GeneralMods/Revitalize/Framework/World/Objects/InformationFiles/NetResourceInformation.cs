using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netcode;

namespace Omegasis.Revitalize.Framework.World.Objects.InformationFiles
{
    public class NetResourceInformation<T> : NetField<T, NetResourceInformation<T>> where T : ResourceInformation, new()
    {

        public NetResourceInformation()
        {

        }

        public NetResourceInformation(T value) : base(value)
        {

        }

        public override void Set(T newValue)
        {
            if (this.canShortcutSet())
                this.value = newValue;
            else if (newValue != this.value)
            {
                this.cleanSet(newValue);
                this.MarkDirty();
            }
        }

        protected override void ReadDelta(BinaryReader reader, NetVersion version)
        {

            if (this.value == null)
                this.value = new T();

            if (version.IsPriorityOver(this.ChangeVersion))
            {
                this.value.readResourceInformation(reader);
                this.setInterpolationTarget(this.value);
            }
        }

        protected override void WriteDelta(BinaryWriter writer)
        {

            if (this.value == null)
                this.value = new T();
            this.value.writeResourceInformation(writer);
        }
    }
}
