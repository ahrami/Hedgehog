using UnityEngine;

[DisallowMultipleComponent]

public class Trigger : MonoBehaviour {

    [SerializeField] protected Trigger[] _conditionTriggers = null;
    protected bool _isActive = false;
    protected Trigger _actionTrigger = null;

	private void Start() {
        if (_conditionTriggers.Length > 0) {
            for (int i = 0; i < _conditionTriggers.Length; ++i) {
                _conditionTriggers[i].SetActionTrigger(this);
            }
        }
    }

	protected bool CheckConditions() {
        if (_conditionTriggers.Length > 0) {
            for (int i = 0; i < _conditionTriggers.Length; ++i) {
                if (_conditionTriggers[i].GetActive() == false) {
                    return false;
                }
            }
        }
        return true;
	}

    public void Activate() {
        _isActive = true;
        if (CheckConditions()) {
            Action();
        }
    }

    protected virtual void Action() {
        if (_actionTrigger) {
            _actionTrigger.Activate();
        }
    }

    public bool GetActive() {
        return _isActive;
	}

    protected void SetActionTrigger(Trigger trigger) {
        _actionTrigger = trigger;

    }
}
