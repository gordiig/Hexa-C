	.section	.data

	.section	.text
	// Prologue
func_foo_start:
	r2 = add(r0, r1);  // First parameter address (topright)
	allocframe(#4000);
	// Pushing symbol x
	r3 = #0;  // Register for holding subtraction value
	r3 = #4;
	r4 = sub(r2, r3);
	r5 = memw(r4);
	// Variable b declaration
	memw(SP + #0) = r5;
	// Getting constant 3
	memw(SP + #4) = #0;
	r0 = #3;
	r1 = add(SP, #4);
	// Getting constant 0
	memw(r1) = r0;
	r0 = #0;
	// Return operator
	r0 = r0;
	jump func_foo_end;
func_foo_end:
	dealloc_return;
	// Prologue
func_main_start:
	// Getting constant 0
	allocframe(#4000);
	r0 = #0;
	memw(SP + #0) = r0;
	r0 = add(SP, #0);  // Current stack head
	r1 = #4;  // Offset from stack head for first parameter
	// Getting constant 0
	call func_foo_start;
	r2 = #0;
	// Return operator
	r0 = r2;
	jump func_main_end;
func_main_end:
	dealloc_return;
