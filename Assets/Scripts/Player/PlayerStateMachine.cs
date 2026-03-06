public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }

    /// <summary>
    /// 进入第一个状态
    /// </summary>
    /// <param name="_startState">第一个状态</param>
    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    /// <summary>
    /// 退出当前状态 进入下一个状态
    /// </summary>
    /// <param name="_newState">下一个状态</param>
    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
