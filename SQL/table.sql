CREATE TABLE "genre" (
	"id" int NOT NULL PRIMARY KEY,
	"genre" varchar NOT NULL
) WITH (
  OIDS=FALSE
);



CREATE TABLE "feature" (
	"id" int NOT NULL PRIMARY KEY,
	"duration_ms" int NOT NULL,
	"danceability" int NOT NULL,
	"energy" int NOT NULL,
	"speechiness" int NOT NULL,
	"acousticness" int NOT NULL,
	"instrumentalness" int NOT NULL,
	"liveness" int NOT NULL,
	"valence" int NOT NULL,
	"loudness" int NOT NULL,
	"tempo" int NOT NULL,
	CONSTRAINT "feature_pk" PRIMARY KEY ("id")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "song" (
	"id" varchar NOT NULL,
	"name" varchar NOT NULL,
	"artist" varchar NOT NULL,
	"song_path" varchar NOT NULL,
	"feature_id" int NOT NULL,
	"user_id" int NOT NULL,
	CONSTRAINT "song_pk" PRIMARY KEY ("id")
) WITH (
  OIDS=FALSE
);

CREATE TABLE "user" (
	"id" int NOT NULL,
	"name" varchar NOT NULL,
	"password" varchar NOT NULL
) WITH (
  OIDS=FALSE
);



CREATE TABLE "playlist" (
	"id" int NOT NULL,
	"name" varchar NOT NULL,
	"created" TIMESTAMP NOT NULL,
	"modified" TIMESTAMP NOT NULL,
	"user_id" int NOT NULL,
	"playlist_content" int NOT NULL,
	CONSTRAINT "playlist_pk" PRIMARY KEY ("id")
) WITH (
  OIDS=FALSE
);



CREATE TABLE "genre_song" (
	"genre_id" int NOT NULL,
	"song_id" int NOT NULL
) WITH (
  OIDS=FALSE
);



CREATE TABLE "history" (
	"song_id" int NOT NULL,
	"user_id" int NOT NULL
) WITH (
  OIDS=FALSE
);



CREATE TABLE "playlist_content" (
	"id" int NOT NULL,
	"song_id" int NOT NULL
) WITH (
  OIDS=FALSE
);





ALTER TABLE "song" ADD CONSTRAINT "song_fk0" FOREIGN KEY ("feature_id") REFERENCES "feature"("id");
ALTER TABLE "song" ADD CONSTRAINT "song_fk1" FOREIGN KEY ("user_id") REFERENCES "user"("id");


ALTER TABLE "playlist" ADD CONSTRAINT "playlist_fk0" FOREIGN KEY ("user_id") REFERENCES "user"("id");
ALTER TABLE "playlist" ADD CONSTRAINT "playlist_fk1" FOREIGN KEY ("playlist_content") REFERENCES "playlist_content"("id");

ALTER TABLE "genre_song" ADD CONSTRAINT "genre_song_fk0" FOREIGN KEY ("genre_id") REFERENCES "genre"("id");
ALTER TABLE "genre_song" ADD CONSTRAINT "genre_song_fk1" FOREIGN KEY ("song_id") REFERENCES "song"("id");

ALTER TABLE "history" ADD CONSTRAINT "history_fk0" FOREIGN KEY ("song_id") REFERENCES "song"("id");
ALTER TABLE "history" ADD CONSTRAINT "history_fk1" FOREIGN KEY ("user_id") REFERENCES "user"("id");

ALTER TABLE "playlist_content" ADD CONSTRAINT "playlist_content_fk0" FOREIGN KEY ("song_id") REFERENCES "song"("id");








