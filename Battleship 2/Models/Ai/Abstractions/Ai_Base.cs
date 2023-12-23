using Battleship_2.Exceptions;
using Battleship_2.Models.FieldComponents;

namespace Battleship_2.Models.Ai.Abstractions
{
    internal abstract class Ai_Base
    {
        public IAi_FindTargetModule? FindTargetModule { get; private set; }
        public IAi_DestroyShipModule? DestroyShipModule { get; private set; }

        private bool _aiHasFoundShip;
        private Cell _lastOpenedCell;

        public Ai_Base()
        {
            FindTargetModule = null;
            DestroyShipModule = null;
            _aiHasFoundShip = false;
            _lastOpenedCell = Cell.NotValid;
        }

        public virtual bool Shoot()
        {
            if (FindTargetModule == null || DestroyShipModule == null) throw new AiException("Modules are not set");
            Ai_TurnInfo info;
            if (_aiHasFoundShip)
            {
                info = DestroyShipModule.DestroyShip(_lastOpenedCell);
                _aiHasFoundShip = !info.WasShipDestroyed;
                _lastOpenedCell = info.LastOpenedCell;
            }
            else
            {
                info = FindTargetModule.FindTarget();
                _aiHasFoundShip = info.WasShotSuccessfull && !info.WasShipDestroyed;
                _lastOpenedCell = info.LastOpenedCell;

            }
            return info.WasShotSuccessfull;
        }

        protected virtual void SetModules(IAi_FindTargetModule findTargetModule, IAi_DestroyShipModule destroyShipModule)
        {
            FindTargetModule = findTargetModule;
            DestroyShipModule = destroyShipModule;
        }
    }
}
