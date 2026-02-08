# Languages & Automata

This repository contains projects for Theory of Languages and Automata. It includes simulations and algorithms for Turing Machines, DFA minimization, NFA-to-DFA conversion, and a Finite State Machine (FSM) coffee machine.

---

## **Projects Included**

### 1. Turing Machine
**Description:** Simulates a single-tape Turing Machine.  

**Features:**
- Supports arbitrary input strings.
- Handles defined state transitions, tape movement, and halting conditions.

---

### 2. DFA Minimizer
**Description:** Minimizes a DFA using equivalence classes.  

**Features:**
- Reads DFA states, transitions, start state, and final states from console input.
- Produces a minimized DFA:
  - Reduced number of states
  - Updated transitions
  - Mapped start and final states

---

### 3. NFA to DFA Converter
**Description:** Converts a NFA with epsilon (ε) transitions into an equivalent DFA.

**Features:**
- Handles epsilon transitions.
- Implements subset construction with BFS.
- Supports trap states for undefined transitions.

---

### 4. Coffee Machine FSM
**Description:** Implements a finite state machine (FSM) to simulate a coffee machine. Outputs indicate the current state, allowing observation of brewing and dispensing stages.

**States:** IDLE, BREWING, DISPENSING  

**Features:**
- Simple console-based simulation.
- Prints the current state, allowing observation of brewing and dispensing stages.