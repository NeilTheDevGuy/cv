using CV.Domain.Enums;

namespace CV.Domain.Objects;

public interface ILookableObject
{
    string Title { get; set; }
    string Description { get; set; }
    ObjectTextType TextType { get; set; }
    List<string> LookableWords { get; set; }
}
