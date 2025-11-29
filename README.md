# Cruce-Card-Game-Simulator

A **C#** implementation of the traditional Transylvanian card game **Cruce** (a variation of Cross/Schnapsen played in teams). This project simulates a full game session for 4 players.

## 📝 Project Overview
This application serves as a game engine that enforces rules and manages the game state. It does not include an AI player; instead, it allows for a manual simulation of all 4 players' turns (hot-seat mode), focusing on strict rule validation and automatic scoring.

## ✨ Key Features

* **Deck Management:** Handles the specific 24-card deck.
* **Bidding System:** Simulates the pre-game bidding phase to determine the Trump suit and target points.
* **Game Logic:**
    * Manages the 6 tricks per round.
    * **Rule Validation:** Enforces suit following and trumping rules (obligatory cut).
    * Winner determination for each trick.
* **Scoring Engine:**
    * Calculates "small points" based on card values.
    * Converts scores to "big points" (33 small points = 1 big point).
    * Verifies if the bidding team fulfilled their contract.

## 🛠️ Tech Stack
* **Language:** C#
* **Framework:** .NET
* **Concepts:** OOP (Classes, Enums, Inheritance), Collections.

## 📖 Rules
The game follows the standard 4-player Cruce rules using the German-suited deck patterns (Red, Bell, Green, Acorn).
Reference: [Tromf.ro Tutorial](https://tromf.ro/tutorial.htm)
