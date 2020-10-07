	.section	.data

	.section	.text

func_main_start:
	// Prologue
	allocframe(#4000)
	// End of prologue
	// Variable z declaration
	memw(SP + #0) = #0;
	// Variable x declaration
	memw(SP + #4) = #0;
	// Setting value for variable x
	// Getting constant 3
	r0 = #3;
	r1 = add(SP, #4)
	memw(r1) = r0;
	// Variable y declaration
	memw(SP + #8) = #0;
	// Setting value for variable y
	// Getting constant 5
	r0 = #5;
	r1 = add(SP, #8)
	memw(r1) = r0;
	// Getting variable y address
	r0 = add(SP, #8)
	r1 = memw(r0);
	// Getting variable x address
	r0 = add(SP, #4)
	r2 = memw(r0);
	// Doing additive operator
	r0 = add(r2, r1);
	// Getting variable "z" for lvalue
	r1 = add(SP, #0)
	r2 = memw(r1);
	// Assigning with some assign operator
	r2 = r0;
	memw(r1) = r2;
	// Getting variable y address
	r0 = add(SP, #8)
	r1 = memw(r0);
	// Getting variable x address
	r0 = add(SP, #4)
	r2 = memw(r0);
	// Doing additive operator
	r0 = sub(r2, r1);
	// Getting variable "z" for lvalue
	r1 = add(SP, #0)
	r2 = memw(r1);
	// Assigning with some assign operator
	r2 = r0;
	memw(r1) = r2;
	// Value for unary operation:
	// Getting variable x address
	r0 = add(SP, #4)
	r1 = memw(r0);
	// Unary -
	r0 = neg(r1);
	// Getting variable "z" for lvalue
	r1 = add(SP, #0)
	r2 = memw(r1);
	// Assigning with some assign operator
	r2 = r0;
	memw(r1) = r2;
	// Getting variable y address
	r0 = add(SP, #8)
	r1 = memw(r0);
	// Getting variable x address
	r0 = add(SP, #4)
	r2 = memw(r0);
	// Doing & operator
	r0 = and(r2, r1);
	// Getting variable "z" for lvalue
	r1 = add(SP, #0)
	r2 = memw(r1);
	// Assigning with some assign operator
	r2 = r0;
	memw(r1) = r2;
	// Getting variable y address
	r0 = add(SP, #8)
	r1 = memw(r0);
	// Getting variable x address
	r0 = add(SP, #4)
	r2 = memw(r0);
	// Doing | operator
	r0 = or(r2, r1);
	// Getting variable "z" for lvalue
	r1 = add(SP, #0)
	r2 = memw(r1);
	// Assigning with some assign operator
	r2 = r0;
	memw(r1) = r2;
	// Value for unary operation:
	// Getting variable x address
	r0 = add(SP, #4)
	r1 = memw(r0);
	// Unary ~
	r0 = not(r1);
	// Getting variable "z" for lvalue
	r1 = add(SP, #0)
	r2 = memw(r1);
	// Assigning with some assign operator
	r2 = r0;
	memw(r1) = r2;
	// Getting return value
	// Getting constant 0
	r0 = #0;
	r0 = r0;
	// Return operator
	jump func_main_end;
func_main_end:
	dealloc_return;

