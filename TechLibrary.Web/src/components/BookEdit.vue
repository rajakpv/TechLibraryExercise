<template>
<form  id="bookStore"  @submit="validateForm()">    
    <div >
      <div>
        <h2>First Name</h2>
        <input type="text" class="form-control form-control-sm" v-model="book.title" placeholder="edit me"/>
      </div>

      <div>
        <h2>ISBN</h2>
        <input type="text" class="form-control form-control-sm" v-model="book.isbn" placeholder="edit me"/>
      </div>

       <div>
        <h2>Description</h2>        
        <textarea class="md-textarea form-control" v-model="book.descr"/>
      </div>

      <div>
        <h2>Thumbnail Url</h2>
        <textarea class="md-textarea form-control" v-model="book.thumbnailUrl"/>
      </div>
      <div class="modal-footer">
        <button type="submit" class="btn btn-primary" @click.stop.prevent="submit(book)" >Save changes</button>
        <button type="button" class="btn btn-secondary" data-dismiss="modal" @click="Cancel()">Cancel</button>       
      </div>
    </div>
    </form>
</template>

<script>
import axios from "axios";

export default {
  name: "Book",
  props: ["id"],
  data: () => ({
    book: {}
  }),
  mounted() {
    axios.get(`https://localhost:5001/books/${this.id}`).then(response => {
      if (response.data != null && response.data != "") {
        this.book = response.data;
      } else {
        this.book = {};
      }
    });
  },

  methods: {
    validateForm: function(e) {
      if (this.book.title && this.book.isbn) {
        return true;
      }

      this.errors = [];

      if (!this.book.title) {
        this.errors.push("Title required.");
      }
      if (!this.book.isbn) {
        this.errors.push("ISBN required.");
      }

      e.preventDefault();
    },

    submit(book1) {
      var test = book1;
      book1.ShortDescr = book1.descr;
      if (book1.bookId > 0) {
        axios({
          method: "post",
          url: "https://localhost:5001/Books/UpdateBook/",
          data: book1
        });
      } else {
        axios({
          method: "post",
          url: "https://localhost:5001/Books/AddBook/",
          data: book1
        });
      }
      this.$router.push("/");
    },
    Cancel() {
      this.$router.push("/");
    }
  }
};
</script>