namespace NoviCode;

// A serialisable description of WHICH fund operation to run. A command carries this value
// (data), and the handler maps it to the matching IFundsStrategy (behaviour). This is the
// CQRS-friendly replacement for passing an IFundsStrategy instance into the old service.
public enum FundsOperation
{
	Add,
	Subtract,
	ForceSubtract
}
