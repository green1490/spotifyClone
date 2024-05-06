CREATE OR REPLACE PROCEDURE insert_genre(new_song_id INTEGER, genres VARCHAR[]) AS $$
  DECLARE
  	genre_item VARCHAR;
  	table_genre VARCHAR := null;
  	inserted_id INTEGER;
  BEGIN
  	FOREACH genre_item IN ARRAY genres
    loop
	    -- check if its in the table
	    SELECT name FROM genre
	    INTO table_genre
	   	WHERE name = genre_item;
    	
	   -- if not insert into the table
	   	IF table_genre IS NULL then
	   		insert into genre(name) values (genre_item)
	   		returning id into inserted_id;
		-- if exits get its id
	   	else
	   		select id from genre into inserted_id
	   		where name = table_genre;
	   	END IF;
	   	-- make connection
		insert into genre_song(genre_id,song_id) 
		values (inserted_id, new_song_id);	
    END LOOP;
  END
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE update_modification(playlist_id INTEGER) AS
$$
	BEGIN
		UPDATE playlist 
		SET modified = NOW()
		WHERE id = playlist_id;
	END;
$$ LANGUAGE plpgsql;