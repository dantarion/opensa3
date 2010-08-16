using OpenSALib3.DatHandler;

namespace OpenSALib3.Moveset
{

    class GlideData : DatElement
    {
        public GlideData(DatElement parent, uint offset)
            : base(parent, offset)
        {
            Name = "GlideData";
            Length = 0;
            for (var i = 0; i < 20; i++)
            {
                Children.Add(new GenericElement<float>(this, (uint)(FileOffset + i * 4), "GlideFloat"));
            }
            Children.Add(new GenericElement<int>(this, (FileOffset + 20 * 4), "GlideInt1"));
            Children.Add(new GenericElement<int>(this, (FileOffset + 21 * 4), "GlideInt1"));
        }
    }
}
