using Initializator;

namespace InputSystem
{
    public class PlayerInputSystem : Initializable
    {
        public PlayerInput Input { get; private set; }
        
        private void OnEnable()
        {
            Input.PlayerControls.Enable();
        }

        private void OnDisable()
        {
            Input.PlayerControls.Disable();
        }

        public override void Initialize()
        {
            Input = new PlayerInput();
        }
    }
}