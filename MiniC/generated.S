	.section	.data

	.section	.text
func_foo_start:
	// Prologue
	r2 = add(r0, r1);
	// First parameter address (topright)
	allocframe(#4000);
	r3 = #0;
	// Register for holding subtraction value
	// Pushing symbol x
	r3 = #4;
	r4 = sub(r2, r3);
	r5 = memw(r4);
	memw(SP + #0) = r5;
	// End of prologue
	// Getting return value
	// Getting constant 0
	r0 = #0;
	r0 = r0;
	// Return operator
	jump func_foo_end;
func_foo_end:
	dealloc_return;
func_main_start:
	// Prologue
	allocframe(#4000);
	// End of prologue
	// Getting constant 0
	r0 = #0;
	memw(SP + #0) = r0;
	r0 = add(SP, #0);
	// Current stack head
	r1 = #4;
	// Offset from stack head for first parameter
	call func_foo_start;
	// Getting return value
	// Getting constant 0
	r2 = #0;
	r0 = r2;
	// Return operator
	jump func_main_end;
func_main_end:
	dealloc_return;
