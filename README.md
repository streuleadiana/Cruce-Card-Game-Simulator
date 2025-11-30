# Cruce Card Game Simulator

A robust **C#** implementation of the traditional Transylvanian card game **Cruce** (a sophisticated variation of Cross/Schnapsen played in teams). This project simulates a full game session for 4 players in a hot-seat console environment.

## Project Overview
This application serves as a comprehensive game engine that enforces rules, manages the game state, and handles complex scoring. It is designed for manual simulation of all 4 players' turns, focusing on **strict rule validation** and **automated arithmetic**, allowing players to focus on strategy rather than calculating points.

## Key Features

### Core Gameplay
* **Deck Management:** Custom implementation of the 24-card deck.
* **Bidding System:** Simulates the pre-game bidding phase where players compete to establish the Trump suit.
* **Turn Logic:** Manages the 6 tricks per round with circular player indexing.
* **Game Modes:** Supports both **Short Game** (up to 11 points) and **Long Game** (up to 21 points).

### Advanced Rule Engine
The engine strictly enforces official rules, preventing illegal moves:
* **Follow Suit & Cut:** Players must follow the lead suit. If void in that suit, they are **forced to cut** with a Trump card.
* **First Turn Constraint:** The player who chose the Trump *must* lead with a Trump card in the very first trick (if they hold one).
* **Announcements ("Strigări"):**
    * **Automatic Detection:** The game identifies 20-point (non-trump) and 40-point (trump) pairs (King + Queen).
    * **Leader validation:** Announcements are only awarded if the player is leading the trick ("at hand").

### User Experience (UI)
* **Persistent Scoreboard:** A global dashboard displays the Team Scores, Current Trump, and Contract at every step.
* **Anti-Peeking:** The console clears automatically between turns to ensure players only see their own hand.
* **Customization:** Players can input custom names, generating dynamic team names (e.g., *Team Ana-Ion*).

### Scoring Engine
* Calculates "small points" based on captured cards.
* Adds "Announcement points" to the total.
* Automatically converts totals to "Big Points" (33 small points = 1 Big Point).
* Verifies Contract fulfillment (adds or subtracts points based on the bidding result).

## Tech Stack
* **Language:** C#
* **Concepts:**
    * **OOP:** Modular design using `Game`, `Player`, and `Card` classes.
    * **Enums:** Strongly typed Suits and Values.
    * **Collections:** Usage of Lists and Tuples for hand and trick management.
    * **Algorithms:** Fisher-Yates shuffle algorithm for random deck distribution.

## Rules Reference
The game follows the standard 4-player Cruce rules.
Reference: [Tromf.ro Tutorial](https://tromf.ro/tutorial.htm)
