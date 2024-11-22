using Godot;

namespace Game.Components;

public partial class BbSimpleStateMachine : Node
{
	private const string NO_STATE = "";
    [Signal] public delegate void StateEnteredEventHandler(string state);
    [Signal] public delegate void StateExitedEventHandler(string state);
    [Export] public string InitialState { get; set; } = "";
    [Export] public string MethodPrefix { get; set; } = "";

    private string _state = NO_STATE;

    public string GetState() => _state;

    public override void _Ready()
    {
        if (!string.IsNullOrEmpty(InitialState)) TransitionTo(InitialState);
    }

    public void TransitionTo(string newState)
    {
        if (newState == _state) return;
        if (!string.IsNullOrEmpty(_state)) ExitState();
        _state = newState;
        EnterState();
    }

    private void EnterState()
    {
        CallDelegateFor("_enter");
        EmitSignal(nameof(StateEntered), _state);
    }

    private void ExitState()
    {
        CallDelegateFor("_exit");
        EmitSignal(nameof(StateExited), _state);
    }

    public override void _Input(InputEvent @event) => CallDelegateFor("_input", @event);
    public override void _UnhandledInput(InputEvent @event) => CallDelegateFor("_unhandled_input", @event);
    public override void _UnhandledKeyInput(InputEvent @event) => CallDelegateFor("_unhandled_key_input", @event);

    private Variant? CallDelegateFor(string builtin, Variant? data = null)
	{
    	var method = $"{MethodPrefix}{_state}{builtin}";
    	if (HasMethod(method))
        	return data.HasValue ? Call(method, data.Value) : Call(method);
    	return null;
	}
}
