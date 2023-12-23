using Battleship_2.Models.Ai;
using Battleship_2.Models.FieldComponents;
using System;
using System.Threading;

namespace Battleship_2.Models
{
    internal class PvE_GameManager
    {
        public readonly static int AiTurnDelay = 800;

        private readonly SimpleAiFieldManager _aiFieldManager;
        private readonly PlayerFieldManager _playerFieldManager;
        private bool _isPlayerTurn;

        public event Action? AiFieldChanged;
        public event Action? PlayerFieldChanged;
        public event Action? PlayerWin;
        public event Action? AiWin;
        public event Action<Exception>? ExceptionThrown;

        public PvE_GameManager(SimpleAiFieldManager aiFieldManager, PlayerFieldManager playerFieldManager)
        {
            _aiFieldManager = aiFieldManager;
            _playerFieldManager = playerFieldManager;
            _isPlayerTurn = true;
        }

        public Fleet AiFleet => _playerFieldManager.Field.Fleet;
        public Fleet PlayerFleet => _aiFieldManager.Field.Fleet;
        public FieldCell[,] AiField => _playerFieldManager.Field.Cells;
        public FieldCell[,] PlayerField => _aiFieldManager.Field.Cells;
        public bool IsPlayerWin => _playerFieldManager.Field.Fleet.IsDestroyed;
        public bool IsAiWin => _aiFieldManager.Field.Fleet.IsDestroyed;

        public bool IsPlayerTurn
        {
            get { return _isPlayerTurn; }
            private set
            {
                _isPlayerTurn = value;
                AiFieldChanged?.Invoke();
            }
        }

        public void PlayerShoot(Cell cell)
        {
            try
            {
                IsPlayerTurn = false;

                if (PlayerTurn(cell))
                {
                    if (IsPlayerWin) PlayerWin?.Invoke();
                    IsPlayerTurn = true;
                    return;
                }

                Delay();
                while (AiTurn())
                {
                    if (IsAiWin) AiWin?.Invoke();
                    Delay();
                }
            }
            catch (Exception ex)
            {
                ExceptionThrown?.Invoke(ex);
            }
            finally
            {
                IsPlayerTurn = true;
            }
        }

        private bool PlayerTurn(Cell cell)
        {
            bool isHit = _playerFieldManager.Shoot(cell);
            AiFieldChanged?.Invoke();
            return isHit;
        }

        private bool AiTurn()
        {
            bool isHit = _aiFieldManager.Shoot();
            PlayerFieldChanged?.Invoke();
            return isHit;
        }

        private void Delay()
        {
            Thread.Sleep(AiTurnDelay);
        }
    }
}
