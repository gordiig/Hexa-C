	.section	.data

globalX:
	.int 0, 0, 0

	.section	.text
	// Getting constant 3
	r0 = #3;
	r1 = ##globalX
	memw(r1 + #8) = r0;  // Assigned globalX[2]
	// Getting constant 2
	r0 = #2;
	r1 = ##globalX
	memw(r1 + #4) = r0;  // Assigned globalX[1]
	// Getting constant 1
	r0 = #1;
	r1 = ##globalX
	memw(r1 + #0) = r0;  // Assigned globalX[0]
func_main_start:
	// Prologue
	allocframe(#4000);
	// Variable z declaration
	memw(SP + #0) = #0;
	// Variable x declaration
	memw(SP + #4) = #0;
	// Getting constant 1
	r0 = #1;
	r1 = add(SP, #4);
	memw(r1) = r0;
	// Variable y declaration
	memw(SP + #8) = #0;
	// Getting constant 5
	r0 = #5;
	r1 = add(SP, #8);
	memw(r1) = r0;
	// Getting variable x address
	r0 = add(SP, #4);
	r1 = memw(r0);
	// Getting variable y address
	r0 = add(SP, #8);
	r2 = memw(r0);
	// Saving r0-r2 before sfdiv
	r3 = r0;
	r4 = r1;
	r5 = r2;
	// Calling __hexagon_umodsi3
	r0 = r2;
	r1 = r1;
	call __hexagon_umodsi3;
	// Restoring r1-r2 registers
	r1 = r4;
	r2 = r5;
	// Getting variable "z" for lvalue
	r1 = add(SP, #0);
	r2 = memw(r1);
	// Assigning with some assign operator
	r2 = r0;
	memw(r1) = r2;
	// Getting constant 0
	r0 = #0;
	r0 = r0;
	// Return operator
	jump func_main_end;
func_main_end:
	dealloc_return;
