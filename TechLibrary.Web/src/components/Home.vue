<template>
  <div>
     <div class="col-md-8"><br/></div>
          <div class="col-md-8">
      <div class="input-group mb-3">
        <input
          type="text"
          class="form-control"
          placeholder="Search by title"
          v-model="searchTitle"
        />
        <div class="input-group-append">
          <button
            class="btn btn-outline-secondary"
            type="button"
            items="dataContext" 
            @click="handleSearchClickEvent($event)"
          >
            Search
          </button>
        </div>
      </div>
    </div>
      <div class="col-md-12">
      <div class="mb-3">
        Items per Page:
        <select v-model="perPage" @change="handlePageSizeChange($event)">
          <option v-for="size in pageSizes" :key="size" :value="size">
            {{ size }}
          </option>
        </select>
      </div>
      </div>
    <div class="home overflow-auto">
        <h1>{{ msg }}</h1>
       <b-pagination v-model="currentPage" :total-rows="totalRows" :per-page="perPage" @click="handlePageSizeChange($event)"></b-pagination>
 
       <p class="mt-3 align-middle">Current Page: {{ currentPage }}
       <button type="button" class="btn btn-secondary float-sm-right" data-dismiss="modal" @click="AddNewBook()">Add NewBook</button></p>
        <b-table ref="table" id="my-table" striped hover :items="dataContext" :fields="fields" :per-page="perPage" :current-page="currentPage" responsive="sm">
            <template v-slot:cell(thumbnailUrl)="data">
                <b-img :src="data.value" thumbnail fluid></b-img>
            </template>
            <template v-slot:cell(title_link)="data">
                <b-link :to="{ name: 'book_view', params: { 'id' : data.item.bookId } }">{{ data.item.title }}</b-link>
            </template> 
            <template  v-slot:cell(Edit)="data">              
              <b-button variant="primary"
              :to="{ name: 'bookedit_view', params: { 'id' : data.item.bookId } }"
               size="sm">Edit</b-button>
            </template>
            <template slot="actions" slot-scope="data">
      <button class="btn btn-dark" :ref="'btn' + data.bookId">Update</button>
    </template>
        </b-table>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "Home",
  props: {
    msg: String
  },
  data: () => ({
    page: 1,
    currentTutorial: null,
    currentIndex: -1,
    searchTitle: "",
    pageSize: 10,
    perPage: 20,
    totalRows: 500,
    currentPage: 1,

    pageSizes: [10, 25, 50, 100],
    fields: [
      { key: "thumbnailUrl", label: "  Image" },
      {
        key: "title_link",
        label: "Book Title",
        sortable: true,
        sortDirection: "desc"
      },
      { key: "isbn", label: "ISBN", sortable: true, sortDirection: "desc" },
      {
        key: "descr",
        label: "Description",
        sortable: true,
        sortDirection: "desc"
      },
      { key: "Edit" },
      { key: "Delete" }
    ],

    items: [],
    computed: {
      rows() {
        return this.items.length;
      }
    }
  }),
  computed: {
    rows() {
      return this.items.length;
    }
  },

  methods: {
    dataContext(ctx, callback) {
      if (this.searchTitle != "") {
        axios
          .get("https://localhost:5001/books/SearchBook/", {
            params: {
              pageNumber: this.currentPage,
              pageSize: this.pageSize,
              SearchString: this.searchTitle
            }
          })
          .then(response => {
            callback(response.data);
            this.$root.$emit("bv::refresh::table", "table");
          });
      } else {
        axios
          .get("https://localhost:5001/books", {
            params: { pageNumber: this.currentPage, pageSize: this.pageSize }
          })
          .then(response => {
            callback(response.data);
          });
      }
      this.$root.$emit("bv::refresh::table", "table");
    },
    handlePageChange(value) {
      this.page = value;
      this.dataContext;
    },

    handlePageSizeChange(event) {
      this.pageSize = event.target.value;
      this.page = 1;
      this.dataContext;
    },

    handleSearchClickEvent(event, callback) {
      axios
        .get("https://localhost:5001/books/SearchBook/", {
          params: {
            pageNumber: this.currentPage,
            pageSize: this.pageSize,
            SearchString: this.searchTitle
          }
        })
        .then(response => {
          this.$refs.table.items = response.data;
          this.$root.$emit("bv::refresh::table", "table");
        });
    },

    getRequestParams(page, pageSize) {
      let params = {};

      if (page) {
        params["page"] = page - 1;
      }

      if (pageSize) {
        params["size"] = pageSize;
      }

      return params;
    },
    AddNewBook() {
      this.$router.push("/BookEdit/" + 0);
    }
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>

