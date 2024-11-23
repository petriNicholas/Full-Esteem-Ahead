using Godot;

namespace Game.Components;

public partial class BbSimpleStateMachine : Node
{
    [Signal] public delegate void StateEnteredEventHandler(string state);
    [Signal] public delegate void StateExitedEventHandler(string state);
    public string InitialState { get; set; } = "";
    private string MethodPrefix = "";
    [Export] public Node StateOwner { get; set; }

    private string _state = "";

    public string GetState() => _state;

    public override void _Ready()
    {
        if (!string.IsNullOrEmpty(InitialState)) TransitionTo(InitialState);
    }

    public void TransitionTo(string newState)
    {
        GD.Print($"Transitioning from state: {_state} to state: {newState}");
        if (newState == _state) return;
        if (!string.IsNullOrEmpty(_state)) ExitState();
        _state = newState;
        EnterState();
    }

    public void EnterState()
    {
        GD.Print($"Entering state: {_state}");
        CallDelegateFor("_enter");
        EmitSignal(nameof(StateEntered), _state);
    }

    public void ExitState()
    {
        GD.Print($"Exiting state: {_state}");
        CallDelegateFor("_exit");
        EmitSignal(nameof(StateExited), _state);
    }

    // Funkcje te są zakomentowane, gdyż się ciągle wywoływały i śmieciły console log, trzeba zobaczyć czy należy usunąć czy zostawić

    //public override void _Process(double delta) => CallDelegateFor("_process", (float)delta);
    public override void _PhysicsProcess(double delta) => CallDelegateFor("_physics_process", (float)delta);
    //public override void _Input(InputEvent @event) => CallDelegateFor("_input", @event);
    //public override void _UnhandledInput(InputEvent @event) => CallDelegateFor("_unhandled_input", @event);
    //public override void _UnhandledKeyInput(InputEvent @event) => CallDelegateFor("_unhandled_key_input", @event);

    public Variant? CallDelegateFor(string builtin, Variant? data = null)
    {
        string method = $"{MethodPrefix}{_state}{builtin}";
        GD.Print($"Attempting to call method: {method}");

        if (StateOwner != null && StateOwner.HasMethod(method))
            return data.HasValue ? StateOwner.Call(method, data.Value) : StateOwner.Call(method);

        GD.PrintErr($"Method not found: {method}");
        return null;
    }
}
