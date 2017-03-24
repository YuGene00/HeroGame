using System.Collections.Generic;

public class Formula {

	//data structure
	int unitCount;
	Stack<Operation> poolStack;
	List<Operation> tagList;

	//Operation for list
	Operation baseOperation;
	Operation tailOperation;

	//auto calculate flag
	public bool AutoCalculate { get; set; }

	//empty tag
	int emptyTag = 0;
	public int EmptyTag { get { return emptyTag; } }

	//return value
	public float BaseValue { get { return baseOperation.Result; } }
	public float Value { get { return tailOperation.Result; } }

	//Operation which has this Formula
	Operation parentOperation = null;
	public Operation ParentOperation { set { parentOperation = value; } }

	public Formula(int unitCount = 20) {
		this.unitCount = unitCount;
		InitDataStruct();
		AllocateMemory();
		ExpandList();
		InitBaseOperation();
		AutoCalculate = true;
	}

	void InitDataStruct() {
		poolStack = new Stack<Operation>(unitCount);
		tagList = new List<Operation>(unitCount);
	}

	void AllocateMemory() {
		for (int i = 0; i < unitCount; ++i) {
			PushNewOperation();
		}
	}

	void PushNewOperation() {
		Operation operation = new Operation();
		poolStack.Push(operation);
	}

	void ExpandList() {
		for (int i = 0; i < unitCount; ++i) {
			tagList.Add(null);
		}
	}

	void InitBaseOperation() {
		baseOperation = new Operation();
		tailOperation = baseOperation;
		baseOperation.SetValue(0);
	}

	#region public void SetBaseValue(float/Formula value)
	public void SetBaseValue(float value) {
		baseOperation.Operator = Operator.NONE;
		SetValueToOperation(value, baseOperation);
	}

	public void SetBaseValue(Formula value) {
		baseOperation.Operator = Operator.NONE;
		SetValueToOperation(value, baseOperation);
	}
	#endregion

	#region void SetValueToOperation(float/Formula value, Operation operation)
	void SetValueToOperation(float value, Operation operation) {
		operation.SetValue(value);
		CallAutoCalculate(operation);
	}

	void SetValueToOperation(Formula value, Operation operation) {
		operation.SetValue(value);
		CallAutoCalculate(operation);
	}
	#endregion

	void CallAutoCalculate(Operation operation) {
		if (AutoCalculate) {
			CallRecalculate(operation);
		}
	}

	void CallRecalculate(Operation operation) {
		operation.Recalculate();
		if (parentOperation != null) {
			parentOperation.Recalculate();
		}
	}

	public void CalculateAll() {
		CallRecalculate(baseOperation);
	}

	#region public void Set[Operator]ToTag(float/Formula value, int tag)
	public void SetAdditionToTag(float value, int tag) {
		Operation operation = RetainOperationByOperatorAndTag(Operator.ADDITION, tag);
		SetValueToOperation(value, operation);
	}

	public void SetAdditionToTag(Formula value, int tag) {
		Operation operation = RetainOperationByOperatorAndTag(Operator.ADDITION, tag);
		SetValueToOperation(value, operation);
	}

	public void SetSubtractionToTag(float value, int tag) {
		Operation operation = RetainOperationByOperatorAndTag(Operator.SUBTRACTION, tag);
		SetValueToOperation(value, operation);
	}

	public void SetSubtractionToTag(Formula value, int tag) {
		Operation operation = RetainOperationByOperatorAndTag(Operator.SUBTRACTION, tag);
		SetValueToOperation(value, operation);
	}

	public void SetMultiplicationToTag(float value, int tag) {
		Operation operation = RetainOperationByOperatorAndTag(Operator.MULTIPLICATION, tag);
		SetValueToOperation(value, operation);
	}

	public void SetMultiplicationToTag(Formula value, int tag) {
		Operation operation = RetainOperationByOperatorAndTag(Operator.MULTIPLICATION, tag);
		SetValueToOperation(value, operation);
	}

	public void SetDivisionToTag(float value, int tag) {
		Operation operation = RetainOperationByOperatorAndTag(Operator.DIVISION, tag);
		SetValueToOperation(value, operation);
	}

	public void SetDivisionToTag(Formula value, int tag) {
		Operation operation = RetainOperationByOperatorAndTag(Operator.DIVISION, tag);
		SetValueToOperation(value, operation);
	}
	#endregion

	Operation RetainOperationByOperatorAndTag(Operator oper, int tag) {
		if (IsTagEmpty(tag)) {
			InsertOperationToTag(tag);
		}
		Operation operation = GetOperationByTag(tag);
		operation.Operator = oper;
		return operation;
	}

	public bool IsTagEmpty(int tag) {
		return GetOperationByTag(tag) == null;
	}

	public Operation GetOperationByTag(int tag) {
		Operation operation = null;
		if (!IsTagOutOfRange(tag)) {
			operation = tagList[tag];
		}
		return operation;
	}

	bool IsTagOutOfRange(int tag) {
		return tag >= tagList.Count;
	}

	void InsertOperationToTag(int tag) {
		Operation operation = RetainOperation();
		ExpandListToTag(tag);
		tagList[tag] = operation;
		AttachToTail(operation);
		emptyTag = FindEmptyTagFromNext();
	}

	Operation RetainOperation() {
		if (poolStack.Count <= 0) {
			AllocateMemory();
		}
		return poolStack.Pop();
	}

	void ExpandListToTag(int tag) {
		while (IsTagOutOfRange(tag)) {
			ExpandList();
		}
	}

	void AttachToTail(Operation operation) {
		tailOperation.Next = operation;
		tailOperation = tailOperation.Next;
	}

	int FindEmptyTagFromNext() {
		int tag = emptyTag;
		while (true) {
			ExpandListToTag(tag);
			if (IsTagEmpty(tag)) {
				return tag;
			}
			++tag;
		}
	}

	public void removeOperationByTag(int tag) {
		if (IsTagEmpty(tag)) {
			return;
		}
		Operation prevOperation = GetOperationByTag(tag).Prev;
		ReturnOperationToStackByTag(tag);
		CallAutoCalculate(prevOperation);
	}

	void ReturnOperationToStackByTag(int tag) {
		poolStack.Push(GetOperationByTag(tag));
		RemoveOperationFromListByTag(tag);
	}

	void RemoveOperationFromListByTag(int tag) {
		tagList[tag].Exclude();
		tagList[tag].Clear();
		tagList[tag] = null;
		emptyTag = FindEmptyTagAfterRemove(tag);
	}

	int FindEmptyTagAfterRemove(int removeTag) {
		if (removeTag < emptyTag) {
			return removeTag;
		} else {
			return FindEmptyTagFromNext();
		}
	}

	public void Clear() {
		for (int tag = 0; tag < tagList.Count; ++tag) {
			if (!IsTagEmpty(tag)) {
				ReturnOperationToStackByTag(tag);
			}
		}
	}

	public enum Operator { NONE, ADDITION, SUBTRACTION, MULTIPLICATION, DIVISION }

	public class Operation {

		//Operator
		Operator oper = Operator.NONE;
		public Operator Operator { get { return oper; } set { oper = value; } }

		//value for operation
		float floatValue = 0;
		Formula formulaValue = null;
		public float Value { get { return formulaValue == null ? floatValue : formulaValue.Value; } }

		//result
		float result = 0;
		public float Result { get { return result; } }

		//Operation for list
		Operation prev = null;
		public Operation Prev { get { return prev; } }
		Operation next = null;
		public Operation Next {
			get { return next; }
			set {
				next = value;
				if (value != null) {
					value.prev = this;
				}
			}
		}

		public void SetValue(float value) {
			ClearValue();
			floatValue = value;
			result = floatValue;
		}

		public void SetValue(Formula value) {
			ClearValue();
			formulaValue = value;
			value.ParentOperation = this;
			result = formulaValue.Value;
		}

		void ClearValue() {
			floatValue = 0;
			ClearFormula();
			result = 0;
		}

		void ClearFormula() {
			if (formulaValue != null) {
				formulaValue.ParentOperation = null;
				formulaValue = null;
			}
		}

		public void Clear() {
			oper = Operator.NONE;
			ClearValue();
			prev = null;
			next = null;
		}

		public void Recalculate() {
			if (prev != null) {
				CalculateTo(prev.result);
			} else {
				CallNextCalculate();
			}
		}

		void CalculateTo(float prevValue) {
			switch (oper) {
				case Operator.ADDITION:
					result = prevValue + Value;
					break;
				case Operator.SUBTRACTION:
					result = prevValue - Value;
					break;
				case Operator.MULTIPLICATION:
					result = prevValue * Value;
					break;
				case Operator.DIVISION:
					result = prevValue / Value;
					break;
			}
			CallNextCalculate();
		}

		void CallNextCalculate() {
			if (next != null) {
				next.CalculateTo(result);
			}
		}

		public void Exclude() {
			if (prev != null) {
				prev.next = next;
			}
			if (next != null) {
				next.prev = prev;
			}
			prev = null;
			next = null;
		}
	}
}