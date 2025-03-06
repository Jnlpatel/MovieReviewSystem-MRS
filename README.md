# Movie Review System

## Project Overview

The Movie Review System is a web application that allows users to discover, rate, and review movies while interacting with others' opinions. This project combines a passion for cinema with technical development, providing a platform for movie enthusiasts to engage with film content and share their thoughts.

## Features

- Browse, add, and review movies
- Categorize movies by genres
- User dashboard for managing personal reviews
- Trending section highlighting top-rated and most-reviewed movies
- Clean and intuitive user interface
- Seamless navigation across features

## Database Design

The system utilizes five main tables with one-to-many (1-M) and many-to-many (M-M) relationships:

1. Users
2. Movies
3. Genres
4. Reviews
5. MovieGenres (join table)


## Relationships

- Users to Reviews: One-to-Many (1-M)
- Movies to Reviews: One-to-Many (1-M)
- Movies to Genres: Many-to-Many (M-M)

