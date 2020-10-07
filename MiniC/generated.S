	.section	.data

	.section	.text

func_main_start:
	// Prologue
	allocframe(#4000)
	// End of prologue
	// Variable y declaration
	memw(SP + #0) = #0;
	// Variable x declaration
	memw(SP + #4) = #0;
	// Setting value for variable x
	// Getting variable y address
	r0 = add(SP, #0)
	r1 = memw(r0);
	// Getting constant 5
	r0 = #5;
	// Doing additive operator
	r2 = add(r0, r1);
	r0 = add(SP, #4)
	memw(r0) = r2;
	// Getting return value
	// Getting constant 0
	r0 = #0;
	r0 = r0;
	// Return operator
	jump func_main_end;
func_main_end:
	dealloc_return;

