using System;
using System.Collections.Generic;

public class Formula<T> {

	//data structure
	int unitCount;
	Stack<Operation> poolStack;

	//Operation for list
	Operation baseOperation;
	Operation tailOperation;

	//auto calculate flag
	public bool AutoCalculate { get; set; }

	//return value
	public T BaseValue { get { return baseOperation.Result; } }
	public T Value { get { return tailOperation.Result; } }

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
		SetBaseValue(default(T));
	}

	#region public void SetBaseValue(T/Formula value)
	public void SetBaseValue(T value) {
		SetValueToOperation(value, baseOperation);
	}

	public void SetBaseValue(Formula<T> value) {
		SetValueToOperation(value, baseOperation);
	}
	#endregion

	#region void SetValueToOperation(T/Formula value, Operation operation)
	void SetValueToOperation(T value, Operation operation) {
		operation.SetValue(value);
	}

	void SetValueToOperation(Formula<T> value, Operation operation) {
		operation.SetValue(value);
	}
	#endregion

	#region public IOperation CreateAddition(T/Formula value)
	public IOperation CreateAddition(T value) {
		Operation operation = CreateOperationByOperator(Operator.ADDITION);
		SetValueToOperation(value, operation);
		return operation;
	}

	public IOperation CreateAddition(Formula<T> value) {
		Operation operation = CreateOperationByOperator(Operator.ADDITION);
		SetValueToOperation(value, operation);
		return operation;
	}
	#endregion

	#region public IOperation CreateSubtraction(T/Formula value)
	public IOperation CreateSubtraction(T value) {
		Operation operation = CreateOperationByOperator(Operator.SUBTRACTION);
		SetValueToOperation(value, operation);
		return operation;
	}

	public IOperation CreateSubtraction(Formula<T> value) {
		Operation operation = CreateOperationByOperator(Operator.SUBTRACTION);
		SetValueToOperation(value, operation);
		return operation;
	}
	#endregion

	#region public IOperation CreateMultiplication(T/Formula value)
	public IOperation CreateMultiplication(T value) {
		Operation operation = CreateOperationByOperator(Operator.MULTIPLICATION);
		SetValueToOperation(value, operation);
		return operation;
	}

	public IOperation CreateMultiplication(Formula<T> value) {
		Operation operation = CreateOperationByOperator(Operator.MULTIPLICATION);
		SetValueToOperation(value, operation);
		return operation;
	}
	#endregion

	#region public IOperation CreateDivision(T/Formula value)
	public IOperation CreateDivision(T value) {
		Operation operation = CreateOperationByOperator(Operator.DIVISION);
		SetValueToOperation(value, operation);
		return operation;
	}

	public IOperation CreateDivision(Formula<T> value) {
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
		T Value { get; }

		void SetValue(T value);
		void SetValue(Formula<T> value);
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
		T TValue = default(T);
		Formula<T> formulaValue = null;
		public T Value { get { return formulaValue == null ? TValue : formulaValue.Value; } }

		//result
		T result = default(T);
		public T Result { get { return result; } }

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
		Formula<T> parentFormula = null;
		public Formula<T> ParentFormula { get { return parentFormula; } set { parentFormula = value; } }

		public void SetValue(T value) {
			ClearValue();
			TValue = value;
			result = TValue;
			StartAutoCalculate();
		}

		public void SetValue(Formula<T> value) {
			ClearValue();
			formulaValue = value;
			value.parentOperation = this;
			result = formulaValue.Value;
			StartAutoCalculate();
		}

		void ClearValue() {
			TValue = default(T);
			ClearFormula();
			result = default(T);
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

		void CalculateTo(T prevValue) {
			switch (oper) {
				case Operator.ADDITION:
					result = Singletons.GenericCalculator<T>.Instance.Add(prevValue, Value);
					break;
				case Operator.SUBTRACTION:
					result = Singletons.GenericCalculator<T>.Instance.Subtract(prevValue, Value);
					break;
				case Operator.MULTIPLICATION:
					result = Singletons.GenericCalculator<T>.Instance.Multiply(prevValue, Value);
					break;
				case Operator.DIVISION:
					result = Singletons.GenericCalculator<T>.Instance.Divide(prevValue, Value);
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

namespace Singletons {
	public abstract class GenericCalculator<T> {

		//calculators for singleton
		static IntCalculator intCalculator = new IntCalculator();
		static FloatCalculator floatCalculator = new FloatCalculator();
		static DoubleCalculator doubleCalculator = new DoubleCalculator();

		public static GenericCalculator<T> Instance {
			get {
				if (typeof(T) == typeof(int)) {
					return intCalculator as GenericCalculator<T>;
				} else if (typeof(T) == typeof(float)) {
					return floatCalculator as GenericCalculator<T>;
				} else if (typeof(T) == typeof(double)) {
					return doubleCalculator as GenericCalculator<T>;
				}
				throw new Exception("Not Numberic Type Exception");
			}
		}

		public abstract T Add(T value1, T value2);
		public abstract T Subtract(T value1, T value2);
		public abstract T Multiply(T value1, T value2);
		public abstract T Divide(T value1, T value2);
		#region public abstract T ConvertToGenericType(int/float/double value)
		public abstract T ConvertToGenericType(int value);
		public abstract T ConvertToGenericType(float value);
		public abstract T ConvertToGenericType(double value);
		#endregion

		#region class [T]Calculator : GenericCalculator<T>
		class IntCalculator : GenericCalculator<int> {

			public override int Add(int value1, int value2) {
				return value1 + value2;
			}

			public override int Subtract(int value1, int value2) {
				return value1 - value2;
			}

			public override int Multiply(int value1, int value2) {
				return value1 * value2;
			}

			public override int Divide(int value1, int value2) {
				return value1 / value2;
			}

			#region public override int ConvertToGenericType(int/float/double value)
			public override int ConvertToGenericType(int value) {
				return value;
			}

			public override int ConvertToGenericType(float value) {
				return (int)value;
			}

			public override int ConvertToGenericType(double value) {
				return (int)value;
			}
			#endregion
		}

		class FloatCalculator : GenericCalculator<float> {

			public override float Add(float value1, float value2) {
				return value1 + value2;
			}

			public override float Subtract(float value1, float value2) {
				return value1 - value2;
			}

			public override float Multiply(float value1, float value2) {
				return value1 * value2;
			}

			public override float Divide(float value1, float value2) {
				return value1 / value2;
			}

			#region public override float ConvertToGenericType(int/float/double value)
			public override float ConvertToGenericType(int value) {
				return value;
			}

			public override float ConvertToGenericType(float value) {
				return value;
			}

			public override float ConvertToGenericType(double value) {
				return (float)value;
			}
			#endregion
		}

		class DoubleCalculator : GenericCalculator<double> {

			public override double Add(double value1, double value2) {
				return value1 + value2;
			}

			public override double Subtract(double value1, double value2) {
				return value1 - value2;
			}

			public override double Multiply(double value1, double value2) {
				return value1 * value2;
			}

			public override double Divide(double value1, double value2) {
				return value1 / value2;
			}

			#region public override double ConvertToGenericType(int/float/double value)
			public override double ConvertToGenericType(int value) {
				return value;
			}

			public override double ConvertToGenericType(float value) {
				return value;
			}

			public override double ConvertToGenericType(double value) {
				return value;
			}
			#endregion
		}
		#endregion
	}
}