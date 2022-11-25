﻿using CV.Domain.Enums;

namespace CV.Domain.Objects;

public class Door : ILookableObject
{
    public string Title { get; set; }
    public string Description { get; set; }
    public ObjectTextType TextType { get; set; }
    public List<string> LookableWords { get; set; }

    public Door(string title, string description, ObjectTextType textType, List<string> lookableWords)
    {
        Title = title;
        Description = description;
        TextType = textType;
        LookableWords = lookableWords;
    }
}
