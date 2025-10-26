library ieee;
use ieee.std_logic_1164.all;
use ieee.std_logic_unsigned.all;

entity CoffeeMachine is
    port (
        clk : in std_logic;
        reset : in std_logic;
        start : in std_logic;
        is_dispensing : out std_logic;
       is_brewing : out std_logic;
	   is_idle : out std_logic);
	   
end CoffeeMachine;

architecture Behavioral of CoffeeMachine is

    type State is (IDLE, BREWING, DISPENSING);
    signal current_state, next_state : State;

    constant BREW_TIME : INTEGER := 10;
    constant DISPENSE_TIME : INTEGER := 8;
begin

    -- Sequential Logic
    process (clk, reset)
    begin
        if reset = '1' then
            current_state <= IDLE;
        elsif rising_edge(clk) then
            current_state <= next_state;
        end if;
    end process;

    -- Next State Logic
    process (current_state, start, clk)
    	variable brewing_time : INTEGER := 0;
    	variable dispensing_time : INTEGER := 0;
    begin
        next_state <= current_state;

        case current_state is
            when IDLE =>
                if start = '1' then
                    next_state <= BREWING;
                end if;

            when BREWING =>
                if brewing_time = BREW_TIME then
                    next_state <= DISPENSING;
                    brewing_time := 0;
                else
                	brewing_time := brewing_time + 1;
                end if;

            when DISPENSING =>
                if dispensing_time = DISPENSE_TIME then
                    next_state <= IDLE;
                    dispensing_time := 0;
                else
                	dispensing_time := dispensing_time + 1;
                end if;

        end case;
    end process;

    -- Output Logic
    process (current_state)
    begin
        case current_state is
            when IDLE =>
                is_dispensing <= '0';
               is_brewing <= '0';
                is_idle <= '1';

            when BREWING =>
                is_dispensing <= '0';
               is_brewing <= '1';
                is_idle <= '0';

            when DISPENSING =>
                is_dispensing <= '1';
               is_brewing <= '0';
                is_idle <= '0';

        end case;
    end process;

end Behavioral;
