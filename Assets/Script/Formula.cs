using System.Collections.Generic;

public class Formula {

	//data structure
	int unitCount;
	Stack<Operation> poolStack;

	//Operation for list
	Operation baseOperation;
	Operation tailOperation;

	//auto calculate flag
	public bool AutoCalculate { get; set; }

	//return value
	public float BaseValue { get { return baseOperation.Result; } }
	public float Value { get { return tailOperation.Result; } }

	//Operation which has this Formula
	Operation parentOperation = null;

	public Formula(int unitCount = 20) {
		this.unitCount = unitCount;
		poolStack = new Stack<Operation>(unitCount);
		AllocateMemory();
		InitializeBaseOperation();
		AutoCalculate = true;
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

	void InitializeBaseOperation() {
		baseOperation = new Operation();
		tailOperation = baseOperation;
		baseOperation.ParentFormula = this;
		baseOperation.Operator = Operator.NONE;
		SetBaseValue(0);
	}

	#region public void SetBaseValue(float/Formula value)
	public void SetBaseValue(float value) {
		SetValueToOperation(value, baseOperation);
	}

	public void SetBaseValue(Formula value) {
		SetValueToOperation(value, baseOperation);
	}
	#endregion

	#region void SetValueToOperation(float/Formula value, Operation operation)
	void SetValueToOperation(float value, Operation operation) {
		operation.SetValue(value);
	}

	void SetValueToOperation(Formula value, Operation operation) {
		operation.SetValue(value);
	}
	#endregion

	#region public IOperation CreateAddition(float/Formula value)
	public IOperation CreateAddition(float value) {
		Operation operation = CreateOperationByOperator(Operator.ADDITION);
		SetValueToOperation(value, operation);
		return operation;
	}

	public IOperation CreateAddition(Formula value) {
		Operation operation = CreateOperationByOperator(Operator.ADDITION);
		SetValueToOperation(value, operation);
		return operation;
	}
	#endregion

	#region public IOperation CreateSubtraction(float/Formula value)
	public IOperation CreateSubtraction(float value) {
		Operation operation = CreateOperationByOperator(Operator.SUBTRACTION);
		SetValueToOperation(value, operation);
		return operation;
	}

	public IOperation CreateSubtraction(Formula value) {
		Operation operation = CreateOperationByOperator(Operator.SUBTRACTION);
		SetValueToOperation(value, operation);
		return operation;
	}
	#endregion

	#region public IOperation CreateMultiplication(float/Formula value)
	public IOperation CreateMultiplication(float value) {
		Operation operation = CreateOperationByOperator(Operator.MULTIPLICATION);
		SetValueToOperation(value, operation);
		return operation;
	}

	public IOperation CreateMultiplication(Formula value) {
		Operation operation = CreateOperationByOperator(Operator.MULTIPLICATION);
		SetValueToOperation(value, operation);
		return operation;
	}
	#endregion

	#region public IOperation CreateDivision(float/Formula value)
	public IOperation CreateDivision(float value) {
		Operation operation = CreateOperationByOperator(Operator.DIVISION);
		SetValueToOperation(value, operation);
		return operation;
	}

	public IOperation CreateDivision(Formula value) {
		Operation operation = CreateOperationByOperator(Operator.DIVISION);
		SetValueToOperation(value, operation);
		return operation;
	}
	#endregion

	Operation CreateOperationByOperator(Operator oper) {
		Operation operation = RetainOperation();
		operation.ParentFormula = this;
		operation.Operator = oper;
		AttachToTail(operation);
		return operation;
	}

	Operation RetainOperation() {
		if (poolStack.Count <= 0) {
			AllocateMemory();
		}
		return poolStack.Pop();
	}

	void AttachToTail(Operation operation) {
		tailOperation.Next = operation;
		tailOperation = tailOperation.Next;
	}

	public void Clear() {
		bool originAutoCalculate = AutoCalculate;
		AutoCalculate = false;
		for (Operation operation = baseOperation.Next; operation != null; operation = baseOperation.Next) {
			operation.DeleteFromFormula();
		}
		AutoCalculate = originAutoCalculate;
	}

	public void RefreshResult() {
		bool originAutoCalculate = AutoCalculate;
		AutoCalculate = true;
		baseOperation.StartAutoCalculate();
		AutoCalculate = originAutoCalculate;
	}

	void CallParentAutoCalculate() {
		if (parentOperation != null) {
			parentOperation.StartAutoCalculate();
		}
	}

	public enum Operator { NONE, ADDITION, SUBTRACTION, MULTIPLICATION, DIVISION }

	public interface IOperation {

		//Operator
		Operator Operator { get; set; }

		//value for operation
		float Value { get; }

		void SetValue(float value);

		void SetValue(Formula value);

		void DeleteFromFormula();
	}

	class Operation : IOperation {

		//Operator
		Operator oper = Operator.NONE;
		public Operator Operator {
			get { return oper; }
			set {
				oper = value;
				StartAutoCalculate();
			}
		}

		//value for operation
		float floatValue = 0;
		Formula formulaValue = null;
		public float Value { get { return formulaValue == null ? floatValue : formulaValue.Value; } }

		//result
		float result = 0;
		public float Result { get { return result; } }

		//Operation for list
		Operation prev = null;
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

		//Formula which has this Operation
		Formula parentFormula = null;
		public Formula ParentFormula { get { return parentFormula; } set { parentFormula = value; } }

		public void SetValue(float value) {
			ClearValue();
			floatValue = value;
			result = floatValue;
			StartAutoCalculate();
		}

		public void SetValue(Formula value) {
			ClearValue();
			formulaValue = value;
			value.parentOperation = this;
			result = formulaValue.Value;
			StartAutoCalculate();
		}

		void ClearValue() {
			floatValue = 0;
			ClearFormula();
			result = 0;
		}

		void ClearFormula() {
			if (formulaValue != null) {
				formulaValue.parentOperation = null;
				formulaValue = null;
			}
		}

		public void StartAutoCalculate() {
			if (!IsParentFormulaAutoCalculate()) {
				return;
			}

			if (prev != null) {
				CalculateTo(prev.result);
			} else {
				result = Value;
				CallNextCalculate();
			}

			if (parentFormula != null) {
				parentFormula.CallParentAutoCalculate();
			}
		}

		bool IsParentFormulaAutoCalculate() {
			return parentFormula != null && parentFormula.AutoCalculate;
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

		public void DeleteFromFormula() {
			if (parentFormula == null) {
				return;
			}
			Operation nextOperation = next;
			ReturnToStack();
			if (nextOperation != null) {
				nextOperation.StartAutoCalculate();
			}
		}

		void ReturnToStack() {
			parentFormula.poolStack.Push(this);
			Exclude();
			Clear();
		}

		void Exclude() {
			if (prev != null) {
				prev.next = next;
			}
			if (next != null) {
				next.prev = prev;
			}
			prev = null;
			next = null;
		}

		void Clear() {
			oper = Operator.NONE;
			ClearValue();
			prev = null;
			next = null;
			parentFormula = null;
		}
	}
}