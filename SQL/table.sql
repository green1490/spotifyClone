CREATE TABLE genre (
	"id" int PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
	"name" varchar NOT NULL UNIQUE
);

CREATE TABLE users (
	"id" int PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
	"name" varchar NOT NULL UNIQUE,
	"password" varchar NOT NULL
);

CREATE TABLE song (
	"id" int PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
	"name" varchar NOT NULL,
	"artist" varchar NOT NULL,
	"song_data" bytea NOT NULL,
	"user_id" int references users(id)
);

CREATE TABLE playlist (
	"id" int PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
	"name" varchar NOT NULL UNIQUE,
	"created" TIMESTAMP NOT NULL,
	"modified" TIMESTAMP NOT NULL,
	"user_id" int REFERENCES users(id)
);

CREATE TABLE playlist_content (
	"playlist_id" int REFERENCES playlist(id),
	"song_id" int REFERENCES song(id)
);

CREATE TABLE genre_song (
	"genre_id" int REFERENCES genre(id),
	"song_id" int REFERENCES song(id)
);