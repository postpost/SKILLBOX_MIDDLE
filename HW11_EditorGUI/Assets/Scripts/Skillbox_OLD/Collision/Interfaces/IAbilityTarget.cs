using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityTarget : IAbility
{
    // список целей для ловушки
    List<GameObject> Targets { get; set; } //интерфейсы не могут содержать поля, но могут содержать свойства
}
