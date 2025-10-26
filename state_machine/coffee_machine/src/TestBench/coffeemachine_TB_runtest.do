SetActiveLib -work
comp -include "$dsn\src\state_machine.vhd" 
comp -include "$dsn\src\TestBench\coffeemachine_TB.vhd" 
asim +access +r TESTBENCH_FOR_coffeemachine 
wave 
wave -noreg clk
wave -noreg reset
wave -noreg start
wave -noreg is_dispensing
wave -noreg is_brewing
wave -noreg is_idle
# The following lines can be used for timing simulation
# acom <backannotated_vhdl_file_name>
# comp -include "$dsn\src\TestBench\coffeemachine_TB_tim_cfg.vhd" 
# asim +access +r TIMING_FOR_coffeemachine 
