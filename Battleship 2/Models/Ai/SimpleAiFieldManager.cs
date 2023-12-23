using Battleship_2.Models.Ai.Abstractions;
using Battleship_2.Models.FieldComponents;

namespace Battleship_2.Models.Ai
{
    internal class SimpleAiFieldManager : Ai_Base, IFieldManager
    {
        private Field _field;

        public Field Field => _field;

        public SimpleAiFieldManager(Field field)
        {
            _field = field;
            SetModules(
                new Ai_RandomFindTargetModule(field),
                new Ai_SimpleDestroyShipModule(field)
                );
        }
    }
}
