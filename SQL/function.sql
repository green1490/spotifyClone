CREATE OR REPLACE PROCEDURE insert_genre(new_song_id INTEGER, genres VARCHAR[]) AS $$
  DECLARE
  	genre_item VARCHAR;
  	table_genre VARCHAR := null;
  	inserted_id INTEGER;
  BEGIN
  	FOREACH genre_item IN ARRAY genres
    LOOP
	    SELECT name FROM genre
	    INTO table_genre
	   	WHERE name = genre_item;
    	
	   	IF table_genre IS NULL then
	   		insert into genre(name) values (genre_item)
	   		returning id into inserted_id;
	   		
	   		insert into genre_song(genre_id,song_id) 
	   		values (inserted_id, new_song_id);   		
	   	END IF;
    END LOOP;
  END
$$ LANGUAGE plpgsql;