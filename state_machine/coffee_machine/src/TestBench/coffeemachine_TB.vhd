library ieee;
use ieee.std_logic_1164.all;

entity CoffeeMachine_TB is
end CoffeeMachine_TB;

architecture Behavioral of CoffeeMachine_TB is

    constant CLK_PERIOD : time := 10 ns;  -- Clock period
    signal clk : std_logic := '0';        -- Clock signal
    signal reset : std_logic := '0';      -- Reset signal
    signal start : std_logic := '0';      -- Start signal
    signal is_dispensing : std_logic;      -- Coffee ready signal
    signal is_brewing : std_logic;       -- Water ready signal
    signal is_idle : std_logic;           -- Is idle signal

    -- Constants for test case
    constant RESET_TIME : time := 100 ns;
    constant SIM_TIME : time := 500 ns;

begin

    -- Instantiate the unit under test
    uut: entity work.CoffeeMachine
        port map (
            clk => clk,
            reset => reset,
            start => start,
            is_dispensing => is_dispensing,
            is_brewing => is_brewing,
            is_idle => is_idle
        );

    -- Clock process
    clk_process: process
    begin
        while now < SIM_TIME loop
            clk <= '0';
            wait for CLK_PERIOD / 2;
            clk <= '1';
            wait for CLK_PERIOD / 2;
        end loop;
        wait;
    end process;

    -- Stimulus process
    stim_process: process
    begin
        -- Reset the coffee machine
        reset <= '1';
        wait for RESET_TIME;
        reset <= '0';

        -- Wait for a while
        wait for 50 ns;

        -- Start brewing process
		wait for clk_period/2;
        start <= '1';
	   wait for clk_period;
        start <= '0';

        -- Stop the simulation
        wait;
    end process;

end Behavioral;
