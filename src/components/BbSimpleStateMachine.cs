using Godot;

namespace Game.Components;

public partial class BbSimpleStateMachine : Node
{
    [Signal] public delegate void StateEnteredEventHandler(string state);
    [Signal] public delegate void StateExitedEventHandler(string state);
    [Export] public string InitialState { get; set; } = "Walk";
    public string MethodPrefix { get; private set; } = "";
    private Node _stateOwner;

    private string _state = "";

    public string GetState() => _state;

    public override void _Ready()
    {
        _stateOwner = GetParent<CharacterBody2D>();

        if (!string.IsNullOrEmpty(InitialState))
        {
            TransitionTo(InitialState);
        }
    }

    public void TransitionTo(string newState)
    {
        if (newState == _state) return;
        if (!string.IsNullOrEmpty(_state)) ExitState();

        _state = newState;
        EnterState();
    }

    public void EnterState()
    {
        CallDelegateFor("_enter");
        EmitSignal(nameof(StateEntered), _state);
    }

    public void ExitState()
    {
        CallDelegateFor("_exit");
        EmitSignal(nameof(StateExited), _state);
    }

    public override void _Process(double delta) => CallDelegateFor("_process", (float)delta);
    public override void _PhysicsProcess(double delta) => CallDelegateFor("_physics_process", (float)delta);
    public override void _Input(InputEvent @event) => CallDelegateFor("_input", @event);
    public override void _UnhandledInput(InputEvent @event) => CallDelegateFor("_unhandled_input", @event);
    public override void _UnhandledKeyInput(InputEvent @event) => CallDelegateFor("_unhandled_key_input", @event);

    public Variant? CallDelegateFor(string suffix, Variant? data = null)
    {
        if (_stateOwner == null || string.IsNullOrEmpty(_state)) return null;

        string method = $"{MethodPrefix}{_state}{suffix}";

        if (_stateOwner.HasMethod(method))
        {
            return data.HasValue
            ? _stateOwner.Call(method, data.Value)
            : _stateOwner.Call(method);
        }
        return null;
    }
}
